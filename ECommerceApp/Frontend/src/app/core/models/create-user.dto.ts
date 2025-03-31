export interface AddressDto {
  street: string;
  city: string;
  state: string;
  pincode: string;
  country: string;
}

export interface CreateUserDto {
  FullName: string;
  Email: string;
  Password: string;
}
