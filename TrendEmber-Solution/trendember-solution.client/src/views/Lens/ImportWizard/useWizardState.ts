import { useState } from 'react';
import { ImportType } from '../ImportDetails';
import { useImportTradeSets } from '@api/endpoints/trades/hooks';

export const useWizardState = (importType: ImportType | undefined, setShowImportWizard: (value: boolean) => void) => {
    const [name, setName] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [mappingSelections, setMappingSelections] = useState<string>('');
    const [step, setStep] = useState(0);
    const [ignoreFirstRow, setIgnoreFirstRow] = useState<boolean>(false);
    const { mutate } = useImportTradeSets();

    const importFunction = async () => {
        if (!selectedFile) return alert('Please select a file.');

        setIsLoading(true);
        try {
            mutate({ file: selectedFile, name, mapping: mappingSelections, ignoreFirstRow });
            setName('');
            setError(null);
            setShowImportWizard(false);
            setSelectedFile(null);
        } catch {
            setError('Import failed');
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

    return {
        name, setName,
        error, setError,
        isLoading,
        selectedFile, setSelectedFile,
        mappingSelections, setMappingSelections,
        step, setStep,
        ignoreFirstRow, setIgnoreFirstRow,
        importFunction, cancel,
        selectionCriteria: importType === ImportType.WatchList ? ['Symbol'] : ['Symbol', 'Entry Date', 'Exit Date', 'Entry', 'TG1', 'TG2', 'SL'],
        importType
    };
};
