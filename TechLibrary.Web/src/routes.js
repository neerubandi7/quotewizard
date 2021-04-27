import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const Home = () => import(/* webpackChunkName: "Home" */ './components/Home.vue');
const Book = () => import(/* webpackChunkName: "Book" */ './components/Book.vue');
const AddBook = () => import(/* webpackChunkName: "Home" */ './components/AddBook.vue')

const router = new VueRouter({
  mode: 'history',
  routes: [
    { path: '/', component: Home },
    {path: '/add', component: AddBook, name: 'addBook_view', props: true},
    {
      name: 'book_view',
      path: '/book/:id',
      component: Book,
      props: true
    }
  ]
});

export default router;