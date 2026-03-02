export interface GiftDto {
  id:number;
  name: string;
  ticketCost: number;
  description?: string;
  pictureUrl?: string;
  category?: CategoryDto;
  donor: DonorDto;
}

export interface CategoryDto {
    id:number;
    name: string;
}
export interface DonorDto {
    id:number;
    name: string;
    email:string;
}
export interface AddGiftDto {
  name: string;
  ticketCost: number;
  description?: string | null;
  donorId: number;
  categoryId: number;
}

export interface SearchGiftDto
{
 name?:string,
 donorName?:string
 numOfTickets?:number
}




export interface CategoryDto {
  name: string;
}

export interface GiftDto {
    id: number;  
  name: string;
  ticketCost: number;
  description?: string;
  pictureUrl?: string;
  category?: CategoryDto;
  donorName?: string;
}

 
export interface Gift {
  id: number;
  name: string;
  description?: string;
  pictureUrl?: string;
  ticketCost: number;
  category?: { name?: string };
}

