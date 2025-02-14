import {FC, createContext, useState, ReactNode } from 'react';
import ImportDetails, { ImportType } from '../ImportDetails';

interface WizardProviderProps {
    importType: ImportType | undefined;
    setShowImportWizard: (value: boolean) => void;
    children: React.ReactNode;
}
interface WizardContextType {
    name?: string,
    fileName?: string,
    selectionCriteria?: string[],
    importType: ImportType | undefined,
    import: (details: ImportDetails) => Promise<void>;
    cancel: () => void;
    error: string | null;
    isLoading: boolean
    step: number,
    selectedFile: File | null,
    fileContent: string,
    setStepFunc: (stepNumber: number) => void;
    setNameFunc: (name: string) => void;
    setFileNameFunc: (fileName: string) => void;
    setSelectionCriteriaFunc: (selectionCriteria: string[]) => void;
    setSelectedFileFunc: (file: File) => void;
    setFileContentFunc: (fileContent: string) => void;
}

export const WizardContext = createContext<WizardContextType | undefined>(undefined);
const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

export const WizardProvider: FC<WizardProviderProps> = ({ importType, setShowImportWizard, children }) => {
    const [name, setName] = useState<string>('');
    const [fileName, setFileName] = useState('');
    const [selectionCriteria, setSelectionCriteria] = useState<string[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [fileContent, setFileContent] = useState<string>("");
    const [step, setStep] = useState(0);
    const importFunction = async (details: ImportDetails) => {
        setIsLoading(true);
        try {
            console.log("Importing data:", details);
            // Make API call here
            await sleep(3000);
        } catch (err) {
            setError("Import failed");
        } finally {
            setIsLoading(false);
        }
    };

    const cancel = () => {
        setName('');
        setFileName('');
        setSelectionCriteria([]);
        setError(null);
        setShowImportWizard(false);
    };

    return (
        <WizardContext.Provider value={{ 
            name, fileName, selectionCriteria, importType, import:importFunction, 
            cancel, error, isLoading, step, setStepFunc: setStep,
            setNameFunc: setName,
            setFileNameFunc: setFileName,
            setSelectionCriteriaFunc : setSelectionCriteria,
            selectedFile,
            setSelectedFileFunc: setSelectedFile,
            fileContent,
            setFileContentFunc: setFileContent
            }}>
            {children}
        </WizardContext.Provider>
    );
};
