import { IProduct } from "./Product";

export interface IInvoice {
  customerId: number;
  lineItems: ILineLitem[];
  createdOn: Date;
  updatedOn: Date;
}

export interface ILineItem {
  product?: IProduct;
  quantity: number;
}