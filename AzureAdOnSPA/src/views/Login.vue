<template>
    <div class="login">
        <button type="button" @click="onLoginClicked">Login</button>
    </div>
</template>

<script>

    export default {
        name: 'Login',
        props: ['redirectUrl'],
        methods: {
            onLoginClicked: function () {
                this.$azAuthenticationContext.login();
            }
        },
        created: function () {
            var user = this.$azAuthenticationContext.getCachedUser();
            if (user) {
                this.$router.replace(this.redirectUrl);
            }
        }
    }
</script>
