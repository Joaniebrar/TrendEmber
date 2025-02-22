import { useContext } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from '../ImportDetails';

const ImportDefinition = () => {
    const currentContext = useContext(WizardContext);
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            currentContext?.setSelectedFileFunc(file);
            //readFile(file);
        }
    };
    return  (
        <div>
            <h2>Import {currentContext?.importType === ImportType.TradeList ? "Trade List" : "Watch List"}</h2>
            <div className="lens-definition">
                <div className="form-group">
                    <label>Name</label>
                    <input value={currentContext?.name || ''}  onChange={(event)=>currentContext?.setNameFunc(event.target.value)}/>
                </div>
                <div className="form-group">
                    <input type="file" accept=".csv" onChange={handleFileChange} 
                    />
                </div>            
                <div className="form-group">
                    <label>Ignore First Row</label>
                    <input type="checkbox" onChange={(event)=>currentContext?.setIgnoreFirstRowFunc(event.target.checked)}/>
                </div>
            </div>
        </div>
    );
};
export default ImportDefinition;