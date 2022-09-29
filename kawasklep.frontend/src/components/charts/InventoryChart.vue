<template>
  <div v-if="isTimelineBuilt">
    <apexchart
      type="area"
      :width="'100%'"
      height="300"
      :options="options"
      :series="series"
    ></apexchart>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

import VueApexCharts from "vue-apexcharts";
Vue.bind("apexchart", VueApexCharts);

@Options({
  name: "InventoryChart",
  components: {},
})
export default class InventoryChart extends Vue {
  get options() {
    return {
      dataLabels: { enabled: false },
      fill: {
        type: "gradient",
      },
      stroke: {
        curve: "smooth",
      },
      xaxis: {
        categories: this.snapshotTimeline!.timeline,
        type: "datetime",
      },
    };
  }

  get series() {
    return this.$store.getters.snapshotTimeline.productInventorySnapshots.map(
      (snapshot: { productId: any; quantityOnHand: any }) => ({
        name: snapshot.productId,
        data: snapshot.quantityOnHand,
      })
    );
  }

  get isTimelineBuilt() {
    return this.$store.getters.timeLineBuilt;
  }

  get snapshotTimeline() {
    return this.$store.getters.snapshotTimeline;
  }

  async created() {
    await this.$store.dispatch("assignSnapshots");
  }
}
</script>

<style scoped lang="scss"></style>
