import { Input, Label } from "reactstrap";

export function InputCheckbox({ label, onChange, propName, initialValue }) {
  return (
    <>
      <Label check>
        <Input type="checkbox" onChange={(e) => setValue(e, propName, onChange)} value={initialValue} />{' '}
        {label}
      </Label>
    </>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}