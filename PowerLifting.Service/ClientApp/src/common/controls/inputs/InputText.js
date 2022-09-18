import { Input, InputGroup, InputGroupText } from "reactstrap";

export function InputText({ label, onChange, propName, initialValue}) {
  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      <Input onChange={(e) => setValue(e, propName, onChange)} value={initialValue} />
    </InputGroup>
  )
}

export function InputPassword({ label, onChange, propName, initialValue}) {
  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      <Input type="password" onChange={(e) => setValue(e, propName, onChange)} value={initialValue} />
    </InputGroup>
  )
}

export function MultiTextInput({ label, onChange, inputList}) {

  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      {inputList.map(item =>
        <Input onChange={(e) => setValue(e, item.propName, onChange)} value={item.initialValue} />
      )}
    </InputGroup>
  )
}

function setValue(event, propName, onChange) {
  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}