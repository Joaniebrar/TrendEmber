export interface WatchList {
    id: string;
    importedDate: string;
    name: string;
    symbolCount: number;
    agentStatus: string;
}

export interface WatchListSymbol {
    symbol: string;
    name: string;
    market: string;
    lastRecordedPrice: string;
}