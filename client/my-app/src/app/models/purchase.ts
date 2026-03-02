export interface PurchaseDto {
id: number,
  giftName: string;
  quantity: number;
  ticketCost: number;  
  winnerName?: string; 
  }
   export interface BuyerDetailDto {
  name: string;
  userName: string;
  phone: string;
}

export interface PurchaseDetailDto {
  user: BuyerDetailDto;
  purchaseDate: Date;
}

export interface WinnersGiftsReportDto {
  giftName: string;
  giftDescription: string;
  purchaseDate: Date | string | null;
  user: BuyerDetailDto | null;
}