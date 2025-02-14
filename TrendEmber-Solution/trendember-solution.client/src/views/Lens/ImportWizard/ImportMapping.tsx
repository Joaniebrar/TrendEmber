import { useContext } from 'react';
import { WizardContext } from './WizardContext';
import { ImportType } from '../ImportDetails';

const ImportMapping = () => {
    const currentContext = useContext(WizardContext);
    return  (
        <div>
            <h2>{currentContext?.importType === ImportType.TradeList ? "Trade List" : "Watch List"} Mapping</h2>
        </div>
    );
};
export default ImportMapping;