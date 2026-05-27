<script setup lang="ts">
defineProps<{
  anotheraccount: string
}>()
</script>

<template>
  <div>
    <h2>Another account</h2>
    <p>Account: {{ anotheraccount }}</p>
  </div>
</template>
