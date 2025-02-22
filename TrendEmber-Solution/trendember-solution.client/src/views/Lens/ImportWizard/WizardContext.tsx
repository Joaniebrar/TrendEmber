import {FC, createContext, useState } from 'react';
import { ImportType } from '../ImportDetails';
import {useImportTradeSets} from '@api/endpoints/trades/hooks';

interface WizardProviderProps {
    importType: ImportType | undefined;
    setShowImportWizard: (value: boolean) => void;
    children: React.ReactNode;
}
interface WizardContextType {
    name?: string,
    selectionCriteria?: string[],
    importType: ImportType | undefined,
    import: () => Promise<void>;
    cancel: () => void;
    error: string | null;
    isLoading: boolean
    step: number,
    selectedFile: File | null,
    ignoreFirstRow: boolean,
    setStepFunc: (stepNumber: number) => void,
    setNameFunc: React.Dispatch<React.SetStateAction<string>>,
    setSelectedFileFunc: React.Dispatch<React.SetStateAction<File | null>>,
    setIgnoreFirstRowFunc: (ignore: boolean) => void,
    mappingSelections: string | null,
    setMappingSelections: React.Dispatch<React.SetStateAction<string>>,
}

export const WizardContext = createContext<WizardContextType | undefined>(undefined);

export const WizardProvider: FC<WizardProviderProps> = ({ importType, setShowImportWizard, children }) => {
    const [name, setName] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [mappingSelections, setMappingSelections] = useState<string>("");
    const [step, setStep] = useState(0);
    const [ignoreFirstRow, setIgnoreFirstRow] = useState<boolean>(false);
    const { mutate } = useImportTradeSets();

    const importFunction = async () => {
        setIsLoading(true);
        try {
            if (!selectedFile) return alert("Please select a file.");
            
            mutate({ file: selectedFile, name, mapping: mappingSelections, ignoreFirstRow });

        } catch (err) {
            setError("Import failed");
        } finally {
            setIsLoading(false);
        }
    };

    const cancel = () => {
        setName('');
        setError(null);
        setShowImportWizard(false);
        setSelectedFile(null);
    };

    return (
        <WizardContext.Provider value={{ 
            name, 
            selectionCriteria: importType === ImportType.WatchList ? ["Symbol"]: ["Symbol","Entry Date","Exit Date","Entry","TG1","TG2","SL"], 
            importType, import:importFunction, 
            cancel, error, isLoading, step, setStepFunc: setStep,
            setNameFunc: setName,
            selectedFile,
            setSelectedFileFunc: setSelectedFile,
            ignoreFirstRow,
            setIgnoreFirstRowFunc: setIgnoreFirstRow,
            mappingSelections, setMappingSelections
            }}>
            {children}
        </WizardContext.Provider>
    );
};
