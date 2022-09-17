import { ICustomer } from "@/types/Customer";
import { IServiceResponse } from "@/types/ServiceResponse";
import axios from "axios";

/**
 * Customer Service.
 * Provides UI business logic associated with customers.
 */
export class CustomerService {
  API_URL = process.env.VUE_APP_API_URL;

  public async getCustomers(): Promise<ICustomer[]> {
    const result = await axios.get(`${this.API_URL}/api/customer/`);
    return result.data;
  }

  public async addCustomer(
    newCustomer: ICustomer
  ): Promise<IServiceResponse<ICustomer>> {
    const result = await axios.post(
      `${this.API_URL}/api/customer/`,
      newCustomer
    );
    return result.data;
  }

  public async deleteCustomer(customerId: number): Promise<boolean> {
    const result = await axios.delete(
      `${this.API_URL}/api/customer/${customerId}`
    );
    return result.data;
  }
}
