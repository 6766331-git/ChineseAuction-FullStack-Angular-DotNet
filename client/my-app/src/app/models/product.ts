export interface Product{

            id: number;  
          name: string;
          ticketCost: number;
          description?: string;
          pictureUrl?: string;
          category?: string;
          donorName?: string;
            winnerUser?: string;

        }

   export interface CartItem {
  giftId: number;
  giftName: string;
  price: number;
  quantity: number;
  totalPrice: number;
} 