export enum ImportType {
    TradeList = "tradeList",
    WatchList = "watchList"
}

interface ImportDetails {
    type: ImportType,
    fileName: string,
    mapping: Record<string,string>;
}

export default ImportDetails;