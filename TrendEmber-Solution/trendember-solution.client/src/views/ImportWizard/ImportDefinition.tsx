import { useContext } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from './ImportDetails';

const ImportDefinition = () => {
    const currentContext = useContext(WizardContext);
    if (!currentContext) return null;
    
    const { setSelectedFile, setName, name, importType, setIgnoreFirstRow} = currentContext;

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            setSelectedFile(file);
        }
    };
    return  (
        <div>
            <h2>Import {importType === ImportType.TradeList ? "Trade List" : "Watch List"}</h2>
            <div className="lens-definition">
                <div className="form-group">
                    <label>Name</label>
                    <input value={name || ''}  onChange={(event)=>setName(event.target.value)}/>
                </div>
                <div className="form-group">
                    <input type="file" accept=".csv" onChange={handleFileChange} 
                    />
                </div>            
                <div className="form-group">
                    <label>Ignore First Row</label>
                    <input type="checkbox" onChange={(event)=>setIgnoreFirstRow(event.target.checked)}/>
                </div>
            </div>
        </div>
    );
};
export default ImportDefinition;