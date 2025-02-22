import {FC} from 'react';
import { useContext } from 'react';
import ImportDefinition from './ImportDefinition';
import ImportMapping from './ImportMapping';
import { WizardContext } from './WizardContext';


const ImportWizard: FC = () => {    
    const currentContext = useContext(WizardContext);

    return (
        <div className='modal-content'>        
            {currentContext &&
            <>
                {currentContext.step === 0 && <ImportDefinition />}
                {currentContext.step > 0 && <ImportMapping />}
                <div id="import-btns">
                    <button id="cancelImportBtn" onClick={()=> currentContext.cancel()}>Cancel</button>                
                    {currentContext.step == 0 && <button id="nextImportBtn" onClick={()=> currentContext.setStepFunc(1)}>Next</button>}
                    {(currentContext.step > 0) && 
                        <>
                            <button id="prevImportBtn" onClick={()=>currentContext.setStepFunc(0)}>Previous</button>
                            <button id="startImportBtn">Import</button>                       
                        </>
                    }    
                </div>        
            </>}
        </div>
        );
        
};

export default ImportWizard;
