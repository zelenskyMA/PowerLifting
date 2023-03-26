import { UncontrolledTooltip } from 'reactstrap';

export function Tooltip({ text, tooltipTargetId, placement = "top" }) {
  if (!text) { return (<></>); }

  return (
    <UncontrolledTooltip placement={placement} target={tooltipTargetId}>
      <p>{text}</p>
    </UncontrolledTooltip>
  );
}