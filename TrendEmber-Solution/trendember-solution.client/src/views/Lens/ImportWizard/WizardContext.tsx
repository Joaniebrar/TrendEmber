import { FC, createContext } from 'react';
import { useWizardState } from './useWizardState';
import { ImportType } from '../ImportDetails';

interface WizardProviderProps {
    importType: ImportType | undefined;
    setShowImportWizard: (value: boolean) => void;
    children: React.ReactNode;
}

export const WizardContext = createContext<ReturnType<typeof useWizardState> | undefined>(undefined);

export const WizardProvider: FC<WizardProviderProps> = ({ importType, setShowImportWizard, children }) => {
    const wizardState = useWizardState(importType, setShowImportWizard);

    return (
        <WizardContext.Provider value={wizardState}>
            {children}
        </WizardContext.Provider>
    );
};
