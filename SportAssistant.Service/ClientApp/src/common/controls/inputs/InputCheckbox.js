import { Input, Label } from "reactstrap";

export function InputCheckbox({ label, onChange, propName, initialValue, disabled }) {
  return (
    <>
      <Label check>
        <Input disabled={disabled} type="checkbox" onChange={(e) => setValue(e, propName, onChange)} checked={initialValue} />{' '}
        {label}
      </Label>
    </>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.checked; // в value всегда 'on'
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}