import React, { useEffect, useState} from 'react';
import { FaCloudDownloadAlt } from "react-icons/fa"; 
import WatchListItem  from './WatchListItem';
import ImportWizard from '../ImportWizard/ImportWizard';
import { ImportType } from '../ImportWizard/ImportDetails';
import { WizardProvider } from '../ImportWizard/WizardContext';
import { useGetWatchLists } from '@api/endpoints/watchList/hooks';

const Harvester= () => {    
    const { data, error, isLoading, fetchNextPage, hasNextPage, isFetchNextPageError } = useGetWatchLists();
    const [showImportWizard, setShowImportWizard] = useState(false);
    const [importType, setImportType] = useState<ImportType>(ImportType.WatchList);
    const handleScroll = () => {
        if (window.innerHeight + document.documentElement.scrollTop >= document.documentElement.offsetHeight - 200) {
            if (hasNextPage && !isLoading) {
                fetchNextPage();
            }
        }
    };
    useEffect(() => {
        window.addEventListener('scroll', handleScroll);
        
        return () => {
            window.removeEventListener('scroll', handleScroll);
        };
    }, [hasNextPage, isLoading, fetchNextPage]); 
    

        if (isLoading) return <p>Loading...</p>;
        if (error) return <p>Error fetching trade sets</p>;

    return (
        <div className="view-content">
            <h1><FaCloudDownloadAlt /> Harvester</h1>
            {showImportWizard &&
                (<div className="modal-backdrop">
                    <WizardProvider importType={importType} setShowImportWizard={setShowImportWizard}>
                        <ImportWizard />
                    </WizardProvider>
                    </div>
                )}         
            <div id="import-watchlist">
                <button id="import-watchlist-btn" className="import-btn" onClick={() => {setShowImportWizard(true);}}>+ Import Watch List</button>
            </div>
            <div id="watchlist-view">
                <h2>Trade Sets</h2>
                <div
                    className="watchlist-container" 
                   
                >
                    {data && (
                        <div className="watchlist-set">
                            {data.pages.map((page, index) => (
                                <React.Fragment key={index}>
                                    {page.data.map((item) => (
                                        <WatchListItem key={item.id} watchList={item} />
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

export default Harvester;
