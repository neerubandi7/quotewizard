<template>
  <div class="home">
    <input
      type="text"
      name="filterkey"
      placeholder="Filter form here"
      id="filterkey"
      v-model="filterkey"
      v-on:keyup="filterkeyChanged"
    />
    <!-- <button type="submit" @click="addBook">Add book</button> -->
    <b-btn variant="success" @click="addBook">
          Add new Book
        </b-btn>
    <h1>{{ msg }}</h1>
    <!-- {{ rows }} -->
    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
      :sort-by.sync="sortBy"
      :sort-desc.sync="sortDesc"
      responsive="sm"
      @sort-changed="sortChanged"
    >
      <template v-slot:cell(thumbnailUrl)="data">
        <b-img :src="data.value" thumbnail fluid></b-img>
      </template>
      <template v-slot:cell(title_link)="data">
        <b-link
          v-if="editMode && id === data.item.bookId"
          :to="{ name: 'book_view', params: { id: data.item.bookId } }"
        >
          <b-input v-model="data.item.title"> </b-input>
        </b-link>
        <b-link v-else>
          {{ data.item.title }}
        </b-link>
      </template>

      <template v-slot:cell(isbn)="data">
        <b-input type="number" v-if="editMode && id === data.item.bookId" v-model="data.item.isbn"> </b-input>
        <p v-else>{{ data.item.isbn }}</p>
      </template>
      <template v-slot:cell(descr)="data">
        <b-input v-if="editMode && id === data.item.bookId" v-model="data.item.descr"> </b-input>
        <p v-else>{{ data.item.descr }}</p>
      </template>
      <template v-slot:cell(actions)="data">
        <b-button-group v-if="editMode && id === data.item.bookId" >
        <b-btn variant="success" @click="saveEditedRow(data.item)">
          Save
        </b-btn>
        <b-btn variant="danger" @click="cancelEdit()">
          Cancel
        </b-btn>
      </b-button-group>
      <b-btn v-if="!editMode" @click="editRow(data.item)">
        Edit
      </b-btn>
        </template>
    </b-table>
    <b-pagination
      v-model="currentPage"
      :total-rows="totalCount"
      :per-page="perPage"
      aria-controls="my-table"
      @input="pageChange(currentPage)"
    ></b-pagination>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Home",
  props: {
    msg: String,
  },
  mounted() {
    this.getBackendData();
  },
  data: () => ({
    id: null,
    sortBy: "title",
    editMode: false,
    sortDesc: false,
    perPage: 10,
    totalCount: 0,
    optionsObj: {
      sortType: "asc",
      sortField: "title",
      pageNo: 1,
      filterValue: "",
    },
    currentPage: 1,
    rows: 100,
    filterkey: "",
    fields: [
      { key: "thumbnailUrl", label: "Book Image" },
      {
        key: "title_link",
        label: "Book Title",
        sortable: true,
        sortDirection: "desc",
      },
      { key: "isbn", label: "ISBN", sortable: true, sortDirection: "desc" },
      {
        key: "descr",
        label: "Description",
        sortable: true,
        sortDirection: "desc",
      },
      {
        key: "actions",
        label: "Actions",
      },
    ],
    items: [],
  }),

  methods: {
    saveEditedRow(row) {
      axios.put(`https://localhost:5001/books`, row).then(r => {
        //console.log(r);
        this.editMode = false;
        this.id = null;
        this.currentPage = 1;
        this.optionsObj = {
      sortType: "asc",
      sortField: "title",
      pageNo: 1,
      filterValue: "",
    },
        this.getBackendData();
      })
    },
    cancelEdit() {
      this.id = null;
      this.editMode = false;
    },
    editRow(row) {
      this.id = row.bookId;
      this.editMode = true;
    },
    //updateRow(id) {
      //console.log(id);
    //},
    addBook() {
      this.$router.push("/add");
    },
    paramStringify(filterObj) {
      let finalParams = "";
      let params = {};
      let keys = Object.keys(filterObj);
      keys.map((k) => {
        if (filterObj[k]) {
          params = {
            ...params,
            [k]: filterObj[k],
          };
        }
      });
      finalParams = "?";
      keys = Object.keys(params);
      keys.map((k) => {
        finalParams =
          (finalParams === "?" ? finalParams : finalParams + "&") +
          k +
          "=" +
          params[k].toString();
      });
      return finalParams;
    },
    getBackendData() {
      //console.log(this.optionsObj);
      let finalparams = this.paramStringify(this.optionsObj);
      axios
        .get(`https://localhost:5001/books${finalparams}`)
        .then((response) => {
          this.items = response.data.data;
          this.totalCount = response.data.totalRecordCount;
          this.rows = response.data.data.length;
          this.totalRecordsCount = response.data.totalRecordCount
        });
    },
    pageChange(pageNo) {
      if ((pageNo-1) * 10 < this.totalRecordsCount) {
        this.optionsObj.pageNo = pageNo;
        this.getBackendData();
      }
    },
    sortChanged() {
      //console.log(this.sortBy, "test sort", this.sortDesc);
      this.optionsObj.sortType = this.sortDesc ? "asc" : "desc";
      this.optionsObj.sortField = this.sortBy;
      this.optionsObj.pageNo = 1;
      this.currentPage = 1;
      this.getBackendData();
    },
    filterkeyChanged() {
      this.optionsObj = {
        sortType: "asc",
        sortField: "title",
        pageNo: 1,
        filterValue: this.filterkey,
      };
      this.currentPage = 1;
      this.getBackendData();
    },
  },
};
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
