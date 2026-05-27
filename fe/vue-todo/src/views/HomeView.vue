<script setup lang="ts">
import { onMounted, ref } from 'vue'

import Todo from '@/components/Todo.vue'

type TodoItem = {
  id: number
  userId: number
  title: string
  completed: boolean
}

const todos = ref<TodoItem[]>([])

onMounted(async () => {
  const response = await fetch('https://jsonplaceholder.typicode.com/todos')
  const data: TodoItem[] = await response.json()
  todos.value = data.slice(0, 10)
})

const toggleTodo = (id: number, message: string) => {
  alert(message)
  todos.value = todos.value.map((todo) =>
    todo.id === id ? { ...todo, completed: !todo.completed } : todo,
  )
}
</script>

<template>
  <div>
    <h1>Todos</h1>
    <ul>
      <Todo
        v-for="todo in todos"
        :key="todo.id"
        :id="todo.id"
        :user-id="todo.userId"
        :title="todo.title"
        :completed="todo.completed"
        @toggle="toggleTodo"
      />
    </ul>
  </div>
</template>
