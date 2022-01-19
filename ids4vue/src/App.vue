<template>
  <div id="app">
    <button id="login" @click="login">Login</button>
    <button id="api" @click="api">Call API</button>
    <button id="logout" @click="logout">Logout</button>
    <pre id="results">the result</pre>

    <img alt="Vue logo" src="./assets/logo.png">
    <HelloWorld msg="Welcome to Your Vue.js App"/>
  </div>
</template>

<script>
import HelloWorld from './components/HelloWorld.vue'
import * as Oidc from "oidc-client";

export default {
  name: 'App',
  components: {
    HelloWorld
  },
  data() {
    return {
      mgr: new Oidc.UserManager({
        authority: "http://localhost:5000",
        client_id: "js",
        redirect_uri: "http://google.com",
        response_type: "id_token token",
        scope: "openid profile cinema",
        post_logout_redirect_uri: "http://localhost:5003/index.html",
      }),
    }
  },
  methods: {
    log() {
      document.getElementById('results').innerText = '';
      Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
          msg = "Error: " + msg.message;
        } else if (typeof msg !== 'string') {
          msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
      });
    },
    async login() {
      await this.mgr.signinRedirect();
    },

    api() {
      let that = this;
      this.mgr.getUser().then(function (user) {
        this.log(user.access_token)
        let url = "http://localhost:5000/identity";
        let xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
          that.log(xhr.responseText);
          that.log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
      });
    },

    logout() {
      this.mgr.signoutRedirect();
    },
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
