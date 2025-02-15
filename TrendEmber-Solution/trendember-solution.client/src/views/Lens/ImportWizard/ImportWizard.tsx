import {FC} from 'react';
import { useContext } from 'react';
import ImportDefinition from './ImportDefinition';
import ImportMapping from './ImportMapping';
import { WizardContext } from './WizardContext';
import { MdOutlineCancel, MdOutlineNavigateNext, MdOutlineNavigateBefore, MdImportExport  } from 'react-icons/md';

const ImportWizard: FC = () => {    
    const currentContext = useContext(WizardContext);

    return (
        <div className='modal-content'>        
            {currentContext &&
            <>
                {currentContext.step === 0 && <ImportDefinition />}
                {currentContext.step > 0 && <ImportMapping />}
                <div id="import-btns">
                    <button id="cancelImportBtn" onClick={()=> currentContext.cancel()} className='wizardBtn'><MdOutlineCancel className='wizardBtnIcon'/>Cancel</button>                
                    {currentContext.step == 0 && 
                        <button id="nextImportBtn" onClick={()=> currentContext.setStepFunc(1) }  className='wizardBtn'><MdOutlineNavigateNext className='wizardBtnIcon'/>Next</button>}
                    {(currentContext.step > 0) && 
                        <>
                            <button id="prevImportBtn" onClick={()=>currentContext.setStepFunc(0)}  className='wizardBtn'><MdOutlineNavigateBefore className='wizardBtnIcon'/>Previous</button>
                            <button id="startImportBtn"   className='wizardBtn'  ><MdImportExport className='wizardBtnIcon'/>Import</button>                       
                        </>
                    }    
                </div>        
            </>}
        </div>
        );
        
};

export default ImportWizard;
