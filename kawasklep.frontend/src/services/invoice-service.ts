import { IInvoice } from "@/types/IInvoice";
import axios from "axios";

export default class InvoiceService {
  API_URL = process.env.VUE_APP_API_URL;

  public async makeNewInvoice(invoice: IInvoice): Promise<boolean> {
    const now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;
    const result = await axios.post(`${this.API_URL}/api/invoice/`, invoice);
    return result.data;
  }
}
