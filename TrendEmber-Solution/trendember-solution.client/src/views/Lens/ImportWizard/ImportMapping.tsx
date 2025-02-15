import { useContext, useState, useEffect } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from '../ImportDetails';

const ImportMapping = () => {
    const currentContext = useContext(WizardContext);
    const [csvRows, setCsvRows] = useState<string[][]>([]);
  

    useEffect(() => {
        if (currentContext?.selectedFile) {
          const file = currentContext.selectedFile;
          const reader = new FileReader();
      
          reader.onload = (e) => {
            if (e && e.target && typeof e.target.result === 'string') {
              const text = e.target.result;
      
              // Regular expression to correctly split CSV considering commas inside quotes
              const rows = text.split("\n").map((row) => 
                row.match(/(".*?"|[^",\n]+)(?=\s*,|\s*$)/g) || [] // Matches values, taking quoted values into account
              );
      
              setCsvRows(rows.slice(0, 5)); // Show only the first 5 rows
            }
          };
      
          reader.readAsText(file);
        }
      }, [currentContext?.selectedFile]);
      
      function handleSelectChange(event: React.ChangeEvent<HTMLSelectElement>) {
        const selectedValue = event.target.value;
        const allSelects = document.querySelectorAll("select");
    
        allSelects.forEach((select) => {
          if (select !== event.target && select.value === selectedValue) {
            select.value = ""; 
          }
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
                    <select key={`mapping-${index}`} onChange={(e) => handleSelectChange(e)}>
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