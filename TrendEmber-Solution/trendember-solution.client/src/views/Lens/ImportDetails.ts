export enum ImportType {
    TradeList = "tradeList",
    WishList = "wishList"
}

interface ImportDetails {
    type: ImportType,
    fileName: string,
    mapping: Record<string,string>;
}

export default ImportDetails;