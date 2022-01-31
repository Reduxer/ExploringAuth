import Vue from 'vue';
import App from './App.vue';
import * as AuthenticationContext from './assets/adal.js';
import router from './router'

Vue.prototype.$azAuthenticationContext = new AuthenticationContext({
    tenant: process.env.VUE_APP_AZAD_TID,
    clientId: process.env.VUE_APP_AZAD_CID,
    instance: process.env.VUE_APP_AZAD_INSTANCE,
    redirectUri: `${window.location.origin}/signin-callback`,
    postLogoutRedirectUri: `${window.location.origin}`,
    cacheLocation: 'localStorage'
});

Vue.config.productionTip = true;

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
