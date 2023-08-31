import { Input, InputGroup, InputGroupText } from "reactstrap";

export function InputNumber({ label, onChange, propName, initialValue }) {
  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      <Input pattern="[0-9]*" value={initialValue || 0}
        onChange={(e) => setValue(e, propName, onChange)}
        onFocus={(e) => onFocus(e)}
        onBlur={(e) => onFocusLost(e)} />
    </InputGroup>
  )
}

export function MultiNumberInput({ label, onChange, inputList }) {
  return (
    <InputGroup>
      <InputGroupText>{label}</InputGroupText>
      {inputList.map((item, i) =>
        <Input key={'multiInput_' + item.propName + i} pattern="[0-9]*" value={item.initialValue || 0}
          onChange={(e) => setValue(e, item.propName, onChange)}
          onFocus={(e) => onFocus(e)}
          onBlur={(e) => onFocusLost(e)} />
      )}
    </InputGroup>
  )
}

function setValue(event, propName, onChange) {
  var validation = event.target.validity.valid;
  if (!validation) {
    event.preventDefault();
    return;
  }

  //вызываем переданный хендлер, в который уходит: 1) имя свойства в стейте, 2)значение, которое в него надо положить.
  var val = event.target.value;
  if (val == '') {
    onChange(propName, 0);
    return;
  }

  let number = parseInt(val, 10);
  onChange(propName, number);   
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