import React, { useEffect, useState} from 'react';
import { FaChartSimple } from 'react-icons/fa6';
import {useGetTradeSets} from '@api/endpoints/trades/hooks';
import TradeSetItem  from './TradeSetItem';
import ImportWizard from './ImportWizard/ImportWizard';
import { ImportType } from './ImportDetails';

const Lens= () => {
    const { data, error, isLoading, fetchNextPage, hasNextPage, isFetchNextPageError } = useGetTradeSets();
    const [showImportWizard, setShowImportWizard] = useState(false);
    const [importType, setImportType] = useState<ImportType>();
    const handleScroll = () => {
        if (window.innerHeight + document.documentElement.scrollTop >= document.documentElement.offsetHeight - 200) {
            if (hasNextPage && !isLoading) {
                fetchNextPage();
            }
        }
    };
    useEffect(() => {
        window.addEventListener('scroll', handleScroll);

        // Cleanup on unmount
        return () => {
            window.removeEventListener('scroll', handleScroll);
        };
    }, [hasNextPage, isLoading, fetchNextPage]); // Ensure dependencies are correct 
    

        if (isLoading) return <p>Loading...</p>;
        if (error) return <p>Error fetching trade sets</p>;

    return (
        <div className="view-content">
            <h1><FaChartSimple /> Lens</h1>
            {showImportWizard &&
                (<div className="modal-backdrop">
                    <ImportWizard importType = {importType} setShowImportWizard={setShowImportWizard}/>
                    </div>
                )}         
            <div id="import-tradeset">
                <button id="import-tradeset" onClick={() => {setImportType(ImportType.TradeList);setShowImportWizard(true);}}>+ Import</button>
            </div>
            <div id="tradesets-view">
                <h2>Trade Sets</h2>
                <div
                    className="tradeset-container" 
                   
                >
                    {data && (
                        <div className="trade-set">
                            {data.pages.map((page, index) => (
                                <React.Fragment key={index}>
                                    {page.data.map((item) => (
                                        <TradeSetItem key={item.id} tradeSet={item} />
                                    ))}
                                </React.Fragment>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Lens;
