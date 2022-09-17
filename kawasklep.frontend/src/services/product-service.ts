import { IProduct } from "@/types/Product";
import axios from "axios";

/**
 * Product Service.
 * Provides UI business logic associated with producst.
 */
export class ProductService {
  API_URL = process.env.VUE_APP_API_URL;

  public async archive(productId: number) {
    const result = await axios.patch(
      `${this.API_URL}/api/product/${productId}`
    );
    return result.data;
  }

  public async save(newProduct: IProduct) {
    const result = await axios.post(`${this.API_URL}/api/product/`, newProduct);
    return result.data;
  }
}
