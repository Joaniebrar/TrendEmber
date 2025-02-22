import { useContext, useState, useEffect } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from '../ImportDetails';

const ImportMapping = () => {
    const currentContext = useContext(WizardContext);
    const [csvRows, setCsvRows] = useState<string[][]>([]);
    const [selectedValues, setSelectedValues] = useState<string[]>([]);

    useEffect(() => {
        if (currentContext?.selectedFile) {
          const file = currentContext.selectedFile;
          const reader = new FileReader();
      
          reader.onload = (e) => {
            if (e && e.target && typeof e.target.result === 'string') {
              const text = e.target.result;
              const rows = text.split("\n").map((row) => 
                row.match(/(".*?"|[^",\n]+)(?=\s*,|\s*$)/g) || [] 
              );
      
              setCsvRows(rows.slice(0, 5)); 

              // Initialize selectedValues based on column count
              const numberOfColumns = rows[0]?.length ?? 0;
              setSelectedValues((prev) =>
                prev.length === numberOfColumns ? prev : Array(numberOfColumns).fill("")
              );
            }
          };
      
          reader.readAsText(file);
        }
      }, [currentContext?.selectedFile]);
      
      function handleSelectChange(event: React.ChangeEvent<HTMLSelectElement>, index: number) {
        const value = event.target.value;
      
        setSelectedValues((prev) => {
          const updatedValues = [...prev];
      
          while (updatedValues.length <= index) {
            updatedValues.push("");
          }
      
          updatedValues[index] = value;

          const cleanedValues = updatedValues.map((v, i) => (i !== index && v === value ? "" : v));
      
          currentContext?.setMappingSelections(cleanedValues.join(", "));
      
          return cleanedValues; 
        });
      }

    return  (
        <div>
            <h2>{currentContext?.importType === ImportType.TradeList ? "Trade List" : "Watch List"} Mapping</h2>

      {csvRows.length > 0 && (
        <div className="lens-mapping-table-container">
        <table>
        <thead>
            <tr>
                {csvRows[0]?.map((_, index) => (
                <th key={index}>
                    <select key={`mapping-${index}`} className="mapping-selection" value={selectedValues[index] || ""} onChange={(e) => handleSelectChange(e, index)}>
                    <option value="">Select an option</option>
                    {currentContext?.selectionCriteria?.map((option, i) => (
                        <option key={`mapping-${index}-${i}`} value={option}>
                        {option}
                        </option>
                    ))}
                    </select>
                </th>
                ))}
            </tr>
        </thead>   
          <tbody>
            {csvRows.map((row, rowIndex) => (
              <tr key={rowIndex}>
                {row.map((cell, cellIndex) => (
                  <td key={cellIndex}>
                    {cell}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
        </div>
      )}
        </div>
    );
};
export default ImportMapping;