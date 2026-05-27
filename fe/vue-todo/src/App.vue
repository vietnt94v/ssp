<script setup lang="ts"></script>

<template>
  <nav>
    <RouterLink to="/">Home</RouterLink>
    |
    <RouterLink to="/about">About</RouterLink>
    |
    <RouterLink to="/demo">Demo</RouterLink>
    |
    <RouterLink to="/setting">Settings</RouterLink>
  </nav>
  <RouterView />
</template>
