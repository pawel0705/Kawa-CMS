<template>
  <div class="inventory-container">
    <h1 id="invenotryTitle">Inventory Dashboard</h1>
    <hr />
    <div class="inventory-actions">
      <solar-button @click="showNewProductModal" id="addNewBtn">
        Add New Item
      </solar-button>
      <solar-button @click="showShipmentModal" id="receiveShipmentBtn">
        Receive Shipment
      </solar-button>
    </div>
    <table id="inventoryTable" class="table">
      <tr>
        <td>Item</td>
        <td>Quantity On-hand</td>
        <td>Unit Price</td>
        <td>Taxable</td>
        <td>Delete</td>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td>
          {{ item.quantityOnHand }}
        </td>
        <td>
          {{ price(item.product.price) }}
        </td>
        <td>
          <span v-if="item.product.isTaxable"> Yes </span>
          <span v-else> No </span>
        </td>
        <td>
          <div>X</div>
        </td>
      </tr>
    </table>
    <new-product-modal
      v-if="isNewProductVisible"
      @save:product="saveNewProduct"
      @close="closeModals"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      @save:shipment="saveNewShipment"
      inventory="{{inventory}}"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import SolarButton from "@/components/SolarButton.vue";
import { InventoryService } from "@/services/inventory-service";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import { Options, Vue } from "vue-class-component";

const inventoryService = new InventoryService();

@Options({
  name: "Inventory",
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue {
  isNewProductVisible = false;

  isShipmentVisible = false;

  inventory: IProductInventory[] = [];

  price(number: number) {
    if (isNaN(number)) {
      return "-";
    }

    return "$ " + number.toFixed(2);
  }

  closeModals() {
    this.isShipmentVisible = false;
    this.isNewProductVisible = false;
  }

  showNewProductModal() {
    this.isNewProductVisible = true;
  }

  showShipmentModal() {
    this.isShipmentVisible = true;
  }

  saveNewProduct(newProduct: IProduct) {
    console.log(newProduct);
  }

  saveNewShipment(shipment: IShipment) {
    console.log(shipment);
  }

  async fetchData() {
    this.inventory = await inventoryService.getInventory();
  }

  async created() {
    await this.fetchData();
  }
}
</script>

<style scope></style>
