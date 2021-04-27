<template>
  <div class="home">
    {{responseMsg}}
    <form @submit.prevent="submit">
      <div class="form-group">
        <label for="name">Id</label>
        <input
          type="number"
          class="form-control"
          name="bookId"
          id="bookId"
          v-model="fields.bookId"
        />
      </div>

      <div class="form-group">
        <label for="email">Title</label>
        <input
          type="text"
          class="form-control"
          name="title"
          id="title"
          v-model="fields.title"
        />
      </div>
      <div class="form-group">
        <label for="isbn">ISBN</label>
        <input
          type="text"
          class="form-control"
          name="isbn"
          id="isbn"
          v-model="fields.isbn"
        />
      </div>

      <div class="form-group">
        <label for="publishedDate">Published Date</label>
        <input
          type="date"
          class="form-control"
          id="publishedDate"
          name="publishedDate"
          v-model="fields.publishedDate"
        />
      </div>
      <div class="form-group">
        <label for="thumbnailUrl">Thumbnail Url</label>
        <input
          type="text"
          class="form-control"
          name="thumbnailUrl"
          id="thumbnailUrl"
          v-model="fields.thumbnailUrl"
        />
      </div>

      <div class="form-group">
        <label for="shortDescr">Short Descr</label>
        <input
          type="text"
          class="form-control"
          name="shortDescr"
          id="shortDescr"
          v-model="fields.shortDescr"
        />
      </div>

      <div class="form-group">
        <label for="longDesc">Long Desc</label>
        <textarea
          class="form-control"
          name="longDesc"
          id="longDesc"
          rows="5"
          v-model="fields.longDesc"
        ></textarea>
      </div>

      <button type="submit" class="btn btn-primary">Add Book</button>
      <button type="submit" class="btn btn-primary" @click="navigateToHome">Cancel</button>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "AddBook",
  data: () => ({
    fields: {},
    responseMsg: '',

  }),
  mounted() {
  },

  methods: {
    navigateToHome() {
      this.$router.push("/");
    },
    submit() {
      //   this.errors = {};
      this.fields["bookId"] = parseInt(this.fields["bookId"]);
      axios
        .post("https://localhost:5001/books", this.fields)
        .then((response) => {
          if(response.data) {
            this.responseMsg = "Sucessfully saved"
          this.$router.push("/");
          }
        })
        .catch((error) => {
          this.responseMsg = error;
        });
    },
  },
};
</script>