import {FC} from 'react';
import { ImportType } from '../ImportDetails';
import { WizardProvider } from './WizardContext';
import ImportDefinition from './ImportDefinition';
import ImportMapping from './ImportMapping';

interface ImportWizardProps {
    setShowImportWizard: (value: boolean) => void;
    importType: ImportType | undefined;
}

const ImportWizard: FC<ImportWizardProps> = ({importType,setShowImportWizard}) => {    
    return (
        <div className='modal-content'>
        <WizardProvider importType={importType} setShowImportWizard={setShowImportWizard}>
            <ImportDefinition />
            <ImportMapping />
        </WizardProvider>
        </div>
        );
        
};

export default ImportWizard;