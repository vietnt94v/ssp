<script setup lang="ts">
interface Props {
  id: number
  userId: number
  title: string
  completed: boolean
}

const props = defineProps<Props>()

const emit = defineEmits<{
  toggle: [id: number, message: string]
}>()

const handleToggle = () => {
  emit('toggle', props.id, 'hehehe')
}
</script>

<template>
  <li>
    <input type="checkbox" :checked="completed" disabled />
    <span :class="{ completed }">{{ title }}</span>
    <button @click="handleToggle">Toggle</button>
  </li>
</template>

<style scoped>
.completed {
  text-decoration: line-through;
  color: #888;
}
</style>
