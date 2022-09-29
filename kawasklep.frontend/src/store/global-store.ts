import { InventoryService } from "@/services/inventory-service";
import { IInventoryTimeline } from "@/types/InventoryGraph";
import { make } from "vuex-pathify";

class GlobalStore {
  snapshotTimeline: IInventoryTimeline = {
    productInventorySnapshots: [],
    timeline: [],
  };

  isTimelineBuilt = false;
}

const state = new GlobalStore();

const mutations = make.mutations(state);

const actions = {
  async assignSnapshots({ commit }: any) {
    const inventoryService = new InventoryService();
    const res = await inventoryService.getSnapshotHistory();

    const timeline: IInventoryTimeline = {
      productInventorySnapshots: res.productInventorySnapshots,
      timeline: res.timeline,
    };

    commit("SET_SNAPSHOT_TIMELINE", timeline);
    commit("SET_IS_TIMELINE_BUILT", true);
  },
};

const getters = {
  timeLineBuilt() {
    return state.isTimelineBuilt;
  },
  snapshotTimeline() {
    return state.snapshotTimeline;
  },
};

export default {
  state,
  getters,
  mutations,
  actions,
};
