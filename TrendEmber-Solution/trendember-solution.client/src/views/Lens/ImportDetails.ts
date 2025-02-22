export enum ImportType {
    TradeList = "tradeList",
    WatchList = "watchList"
}

interface ImportDetails {
    mapping: Record<string,string>;
}

export default ImportDetails;