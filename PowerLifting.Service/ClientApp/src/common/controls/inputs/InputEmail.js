import { Input, InputGroup, InputGroupText } from "reactstrap";

export function InputEmail({ label, onChange, propName, initialValue }) {
  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      <Input type="email" onChange={(e) => setValue(e, propName, onChange)} value={initialValue} />
    </InputGroup>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}