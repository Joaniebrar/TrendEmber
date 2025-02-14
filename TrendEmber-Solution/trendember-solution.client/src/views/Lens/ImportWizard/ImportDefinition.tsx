import { useContext } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from '../ImportDetails';

const ImportDefinition = () => {
    const currentContext = useContext(WizardContext);
    const readFile = (file: File) => {
        const reader = new FileReader();
        reader.onload = (event) => {
            const text = event.target?.result as string;
            currentContext?.setFileContentFunc(text);
        };
        reader.readAsText(file);
    };
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            currentContext?.setSelectedFileFunc(file);
            readFile(file);
        }
    };
    return  (
        <div>
            <h2>Import {currentContext?.importType === ImportType.TradeList ? "Trade List" : "Watch List"}</h2>
            <label>Name</label>
            <input onChange={(event)=>currentContext?.setNameFunc(event.target.value)}/>
            <input type="file" accept=".csv" onChange={handleFileChange} />
            {currentContext?.selectedFile && <p>Selected File: {currentContext?.selectedFile.name}</p>}
       
        </div>
    );
};
export default ImportDefinition;