<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="UserId" prop="UserId">
          <a-input v-model="entity.UserId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="Name" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="Openid" prop="Openid">
          <a-input v-model="entity.Openid" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="data1" prop="data1">
          <a-input v-model="entity.data1" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="data2" prop="data2">
          <a-input v-model="entity.data2" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="data3" prop="data3">
          <a-input v-model="entity.data3" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="data4" prop="data4">
          <a-input v-model="entity.data4" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
  props: {
    parentObj: Object
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: {},
      rules: {},
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/UserDecilmal/Base_UserDecimal/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/UserDecilmal/Base_UserDecimal/SaveData', this.entity).then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false

            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>
