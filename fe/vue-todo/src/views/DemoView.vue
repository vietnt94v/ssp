<script setup lang="ts">
import { ref } from 'vue'
const list = ref<{ id: number; name: string }[]>([
  {
    id: 1,
    name: 'John Doe',
  },
  {
    id: 2,
    name: 'Jane Doe 2',
  },
  {
    id: 3,
    name: 'Jane Doe 3',
  },
])

const handleChangeOrder = () => {
  list.value = [
    { id: 3, name: 'Jane Doe 3' },
    { id: 2, name: 'Jane Doe 2' },
    { id: 1, name: 'John Doe' },
  ]
}
</script>
<template>
  <div>
    <h1>Demo</h1>
    <ul>
      <li v-for="(item, index) in list" :key="index">
        {{ index + 1 }}
        <input type="text" :value="item.name" />
      </li>
    </ul>
    <button @click="handleChangeOrder">Change order</button>
  </div>
</template>
