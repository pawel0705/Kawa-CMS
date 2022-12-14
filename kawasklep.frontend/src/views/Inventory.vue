<template>
  <div class="inventory-container">
    <h1 id="invenotryTitle">Inventory Dashboard</h1>
    <hr />
    <inventory-chart></inventory-chart>
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
        <td
          v-bind:class="`${applyColor(
            item.quantityOnHand,
            item.idealQuantity
          )}`"
        >
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
          <div
            class="lni lni-cross-circle product-archive"
            @click="archiveProduct(item.product.id)"
          ></div>
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
      :inventory="inventory"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import InventoryChart from "@/components/charts/InventoryChart.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import SolarButton from "@/components/SolarButton.vue";
import { InventoryService } from "@/services/inventory-service";
import { ProductService } from "@/services/product-service";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import { Options, Vue } from "vue-class-component";

const inventoryService = new InventoryService();
const productService = new ProductService();

@Options({
  name: "Inventory",
  components: { SolarButton, NewProductModal, ShipmentModal, InventoryChart },
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

  async archiveProduct(productId: number) {
    await productService.archive(productId);
    await this.initialize();
  }

  async saveNewProduct(newProduct: IProduct) {
    await productService.save(newProduct);
    this.isNewProductVisible = false;
    await this.initialize();
  }

  applyColor(current: number, target: number) {
    if (current <= 0) {
      return "red";
    } else if (Math.abs(target - current) > 8) {
      return "yellow";
    } else {
      return "green";
    }
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

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment);
    this.isShipmentVisible = false;
    await this.initialize();
  }

  async initialize() {
    this.inventory = await inventoryService.getInventory();
    await this.$store.dispatch("assignSnapshots");
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.green {
  font-weight: bold;
  color: $solar-green;
}
.yellow {
  font-weight: bold;
  color: $solar-yellow;
}
.red {
  font-weight: bold;
  color: $solar-red;
}
.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}
.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
