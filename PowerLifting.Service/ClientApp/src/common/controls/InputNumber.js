import { Input, InputGroup, InputGroupText } from "reactstrap";

export function InputNumber({ label, onChange, propName, initialValue }) {

  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      <Input pattern="[0-9]*" value={initialValue}
        onChange={(e) => setValue(e, propName, onChange)}
        onFocus={(e) => onFocus(e)}
        onBlur={(e) => onFocusLost(e)} />
    </InputGroup>
  )
}

function setValue(event, propName, onChange) {
  var validation = event.target.validity.valid;
  if (!validation) {
    event.preventDefault();
    return;
  }

  var val = event.target.value;
  onChange(propName, val); //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
}

function onFocus(event) {
  var val = event.target.value;

  if (val == 0) {
    event.target.value = "";
  }
}

function onFocusLost(event) {
  var val = event.target.value;

  if (val == "") {
    event.target.value = "0";
    return;
  }

  var intVal = parseInt(val);
  event.target.value = intVal.toString();
}