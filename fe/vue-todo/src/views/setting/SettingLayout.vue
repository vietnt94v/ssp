<script setup lang="ts"></script>

<template>
  <div>
    <h1>Settings</h1>
    <nav>
      <RouterLink to="/setting">Overview</RouterLink>
      |
      <RouterLink to="/setting/profile">Profile</RouterLink>
      |
      <RouterLink to="/setting/profile/demo-user">Demo account</RouterLink>
    </nav>
    <RouterView />
  </div>
</template>
