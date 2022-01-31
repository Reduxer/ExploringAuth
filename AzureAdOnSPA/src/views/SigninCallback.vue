<template>
    <p v-if="error">{{error}}</p>
    <p v-else>signing in...</p>
</template>

<script type="text/javascript">
    export default {
        data: function () {
            return {
                error: '',
            }
        },
        created: function () {
            const authContext = this.$azAuthenticationContext;

            var isCallback = authContext.isCallback(window.location.hash);

            authContext.handleWindowCallback();

            if (isCallback && !authContext.getLoginError()) {
                this.$router.replace(authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST));
            }
        }
    }
</script>