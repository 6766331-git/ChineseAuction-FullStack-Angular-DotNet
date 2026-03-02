//models page:
        export interface UserDto {
        UserName: string;
        Name: string;
        Password: string;
        Phone?: string;
        }

        export enum UserStatus {
            user=0,
            admin=1
        }
        export interface BuyerDetailDto {
            name: string;
            email: string;
            phone: string;
        }
     export interface LoginDto {
        UserName: string;
        Password: string;

        }

export interface LoginResponseDto {
  token: string;
  tokenType: string;
  expiresIn: number;
  user: {
    name: string;
    phone: string;
    userName: string;
  };
  role: UserStatus;
}