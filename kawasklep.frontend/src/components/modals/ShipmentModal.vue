<template>
  <solar-modal>
    <template v-slot:header> Receive Shipment </template>
    <template v-slot:body>
      <label for="product">Product Received:</label>
      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="">Please select one</option>
        <option v-for="item in inventory" :value="item" :key="item.product.id">
          {{ item.product.name }}
        </option>
      </select>
      <label for="qtyReceived">Quantity Received:</label>
      <input type="number" id="qtyReceived" v-model="qtyReceived" />
    </template>
    <template v-slot:footer>
      <solar-button
        type="button"
        @button:click="save"
        aria-label="save new shipment"
      >
        Save Received Shipment
      </solar-button>
      <solar-button
        type="button"
        @button:click="close"
        aria-label="Close modal"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { IShipment } from "@/types/Shipment";
import { IProduct, IProductInventory } from "@/types/Product";
import { Vue, Options } from "vue-class-component";
import SolarButton from "../SolarButton.vue";
import SolarModal from "./SolarModal.vue";
import { PropType } from "vue";

@Options({
  name: "ShipmentModal",
  components: { SolarButton, SolarModal },
  props: {
    inventory: { type: Array as PropType<IProductInventory[]>, required: true },
  },
})
export default class ShipmentModal extends Vue {
  selectedProduct: IProduct = {
    createdOn: new Date(),
    updatedOn: new Date(),
    id: 0,
    description: "",
    isTaxable: false,
    name: "",
    price: 0,
    isArchived: false,
  };

  qtyReceived = 0;
  inventory!: IProductInventory[];

  close() {
    this.$emit("close");
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.id,
      adjustment: this.qtyReceived,
    };

    this.$emit("save:shipment", shipment);
  }
}
</script>

<style scoped></style>
