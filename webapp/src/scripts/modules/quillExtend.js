import { Quill } from "@vueup/vue-quill";
import BlotFormatter from 'quill-blot-formatter'
import ImageUploader from 'quill-image-uploader'
import htmlEditButton from "quill-html-edit-button"

if (Quill) {
    Quill.register({
        "modules/htmlEditButton": htmlEditButton
      })
}